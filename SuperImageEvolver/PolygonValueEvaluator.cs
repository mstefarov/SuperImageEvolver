using System;
using System.Collections.Generic;
using System.Drawing;

namespace SuperImageEvolver {
    internal class PolygonValueEvaluator {
        public static ShapeEvaluation[] SortShapes(TaskState state) {
            var results = new List<ShapeEvaluation>();

            var bestMatch = state.BestMatch;
            using (var testCanvas = new Bitmap(state.EvalImageWidth, state.EvalImageHeight)) {
                double baseDivergence = state.Evaluator.CalculateDivergence(testCanvas, bestMatch, state, 1);
                for (int i = 0; i < bestMatch.Shapes.Length; i++) {
                    var dnaWithoutShape = new DNA(bestMatch);
                    dnaWithoutShape.Shapes[i].Color = Color.Transparent;
                    double diffDivergence = state.Evaluator.CalculateDivergence(testCanvas, dnaWithoutShape, state, 1);
                    results.Add(new ShapeEvaluation {
                        Ordinal = i,
                        Divergence = diffDivergence - baseDivergence,
                        Shape = bestMatch.Shapes[i]
                    });
                }
            }

            results.Sort((r1, r2) => Math.Sign(r2.Divergence - r1.Divergence));
            return results.ToArray();
        }
    }


    public class ShapeEvaluation {
        public int Ordinal { get; set; }
        public double Divergence { get; set; }
        public Shape Shape { get; set; }

        public override string ToString() {
            return "ShapeEval(" + Divergence + ")";
        }
    }
}
