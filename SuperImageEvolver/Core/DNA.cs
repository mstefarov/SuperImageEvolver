using System;
using System.Drawing;
using System.IO;

namespace SuperImageEvolver {
    public sealed class DNA : ICloneable {
        public DNA() {}


        public DNA(DNA other) {
            Shapes = new Shape[other.Shapes.Length];
            for (int i = 0; i < Shapes.Length; i++) {
                Shapes[i] = new Shape(other.Shapes[i]);
            }
            Divergence = other.Divergence;
        }


        public object Clone() {
            return new DNA(this);
        }


        public MutationType LastMutation;

        public Shape[] Shapes;
        public double Divergence;


        public DNA(NBTag tag) {
            Divergence = tag["Divergence"].GetDouble();
            var shapesTag = (NBTList)tag["Shapes"];
            Shapes = new Shape[shapesTag.Tags.Length];
            for (int i = 0; i < Shapes.Length; i++) {
                Shapes[i] = new Shape(shapesTag[i]);
            }
        }


        public NBTag SerializeNBT(string tagName) {
            NBTCompound compound = new NBTCompound(tagName);
            compound.Append("Divergence", Divergence);
            NBTList tag = new NBTList("Shapes", NBTType.Compound, Shapes.Length);
            for (int i = 0; i < Shapes.Length; i++) {
                tag[i] = Shapes[i].SerializeNBT();
            }
            compound.Append(tag);
            return compound;
        }


        // Take a polygon and divide it into two, using the slot of a sacrificial polygon for second half
        public void DivideShape(Random rand, int shapeToDivide, int shapeToSacrifice) {
            Shape original = Shapes[shapeToDivide];
            int points = original.Points.Length;

            // Figure out which dividing vertex produces the shortest dividing edge.
            PointF divisionVertex1 = default(PointF);
            PointF divisionVertex2 = default(PointF);
            double shortestDividingEdgeLength = Double.MaxValue;
            Shape shifted = original;
            for (int shiftAmount = 0; shiftAmount < points; shiftAmount++) {
                Shape shiftedGuess = ShiftPoints(original, shiftAmount);
                // Find the two dividing vertices
                PointF vertexGuess1;
                if (points%2 == 1) {
                    // odd number of points
                    vertexGuess1 = shiftedGuess.Points[points/2];
                } else {
                    // even number of points
                    vertexGuess1 = Lerp(shiftedGuess.Points[points/2 - 1], shiftedGuess.Points[points/2], 0.5f);
                }
                PointF vertexGuess2 = Lerp(shiftedGuess.Points[0], shiftedGuess.Points[points - 1], 0.5f);
                double dividingEdgeLength =
                    Math.Sqrt((vertexGuess1.X - vertexGuess2.X)*(vertexGuess1.X - vertexGuess2.X) +
                              (vertexGuess1.Y - vertexGuess2.Y)*(vertexGuess1.Y - vertexGuess2.Y));
                if (dividingEdgeLength < shortestDividingEdgeLength) {
                    shortestDividingEdgeLength = dividingEdgeLength;
                    shifted = shiftedGuess;
                    divisionVertex1 = vertexGuess1;
                    divisionVertex2 = vertexGuess2;
                }
            }

            var half1 = new Shape(shifted);
            var half2 = new Shape(shifted);

            // Construct half-shape #1
            half1.Points[0] = divisionVertex1;
            half1.Points[1] = divisionVertex2;
            for (int i = 0; i < points/2; i++) {
                half1.Points[i + 2] = shifted.Points[i];
            }

            // Construct half-shape #2
            half2.Points[0] = divisionVertex2;
            half2.Points[1] = divisionVertex1;
            for (int i = 0; i < points/2; i++) {
                half2.Points[i + 2] = shifted.Points[points/2 + i];
            }

            // Randomly redistribute extra vertices for each half (if there are any)
            int extraVertices = (points - 3)/2;
            Subdivide(rand, half1, extraVertices);
            Subdivide(rand, half2, extraVertices);

            // Write back the changes
            Shapes[shapeToDivide] = half1;
            Shapes[shapeToSacrifice] = half2;
            half1.OutlineColor = Color.Green;
            half2.OutlineColor = Color.Red;
            ShiftShapeIndex(shapeToSacrifice, shapeToDivide);
        }


        static Shape ShiftPoints(Shape shape, int shift) {
            var copy = new Shape(shape);
            int offset = shift%shape.Points.Length;
            Array.Copy(shape.Points, offset, copy.Points, 0, shape.Points.Length - offset);
            Array.Copy(shape.Points, 0, copy.Points, shape.Points.Length - offset, offset);
            return copy;
        }


        // Randomly redistribute extra vertices by dividing edges of a given polygon
        static void Subdivide(Random rand, Shape shape, int extraVertices) {
            int points = shape.Points.Length;
            int assignedPoints = points - extraVertices;
            while (assignedPoints < points) {
                int p1 = rand.Next(0, assignedPoints);
                if (p1 == assignedPoints - 1) {
                    shape.Points[assignedPoints] = Lerp(shape.Points[p1], shape.Points[0], 0.5f);
                } else {
                    Array.Copy(shape.Points, p1 + 1, shape.Points, p1 + 2, assignedPoints - p1 - 1);
                    shape.Points[p1 + 1] = Lerp(shape.Points[p1], shape.Points[p1 + 2], 0.5f);
                }
                assignedPoints++;
            }
        }


        // Find a point on the Linear intERPolant of two given points, separated from p1 by given amount
        static PointF Lerp(PointF p1, PointF p2, float amount) {
            return new PointF {
                X = p1.X + (p2.X - p1.X)*amount,
                Y = p1.Y + (p2.Y - p1.Y)*amount
            };
        }


        void ShiftShapeIndex(int source, int dest) {
            Shape shape = Shapes[source];
            if (source < dest) {
                for (int i = source; i < dest; i++) {
                    Shapes[i] = Shapes[i + 1];
                }
            } else {
                for (int i = source; i > dest; i--) {
                    Shapes[i] = Shapes[i - 1];
                }
            }
            Shapes[dest] = shape;
        }


        public void SwapShapes(Random rand) {
            int s1 = rand.Next(Shapes.Length);
            Shape shape = Shapes[s1];
            shape.PreviousState = shape.Clone() as Shape;
            if (Shapes.Length < 2) return;
            if (rand.Next(2) == 0) {
                int s2;
                do {
                    s2 = rand.Next(Shapes.Length);
                } while (s1 == s2);
                ShiftShapeIndex(s1, s2);
            } else {
                int s2 = rand.Next(Shapes.Length);
                Shapes[s1] = Shapes[s2];
                Shapes[s2] = shape;
            }
            LastMutation = MutationType.SwapShapes;
        }
    }
}
