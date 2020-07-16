using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace SuperImageEvolver {
    class StateRunner {
        private readonly TaskState state;

        public StateRunner(TaskState state) {
            this.state = state;
        }

        public async Task RunAsync(CancellationToken token) {
            Debug.WriteLine("Running...");
            await Task.Yield();
            Random rand = new Random();
            Bitmap testCanvas = new Bitmap(state.ImageWidth, state.ImageHeight);

            while (!token.IsCancellationRequested) {
                Interlocked.Increment(ref state.Stats.MutationCounter);
                DNA mutation = state.Mutator.Mutate(rand, state.CurrentMatch, state);

                bool takeRisk = (rand.NextDouble() < state.ProjectOptions.RiskRate * state.CurrentMatch.Divergence);
                double riskMargin = -(state.CurrentMatch.Divergence * state.CurrentMatch.Divergence) *
                                    state.ProjectOptions.RiskMargin;
                if (!takeRisk)
                    riskMargin = 0;

                mutation.Divergence = state.Evaluator.CalculateDivergence(testCanvas,
                                                                          mutation,
                                                                          state,
                                                                          state.CurrentMatch.Divergence - riskMargin);

                if (Math.Abs(mutation.Divergence - 1) < float.Epsilon)
                    continue;

                double improvement = state.CurrentMatch.Divergence - mutation.Divergence;

                if (improvement > 0 || takeRisk && (improvement > riskMargin)) {
                    lock (state.ImprovementLock) {
                        riskMargin = -(state.CurrentMatch.Divergence * state.CurrentMatch.Divergence) *
                                     state.ProjectOptions.RiskMargin;
                        if (!takeRisk)
                            riskMargin = 0;
                        mutation.Divergence = state.Evaluator.CalculateDivergence(testCanvas,
                                                                                  mutation,
                                                                                  state,
                                                                                  1);
                        improvement = state.CurrentMatch.Divergence - mutation.Divergence;

                        if (improvement > 0 || takeRisk && (improvement > riskMargin)) {
                            if (improvement <= 0) {
                                if (state.BestMatch.Divergence < state.CurrentMatch.Divergence) {
                                    state.Stats.FailedRiskCounter++;
                                    mutation = state.BestMatch;
                                } else {
                                    state.Stats.RiskyMoveCounter++;
                                }
                            } else {
                                state.Stats.MutationCounts[mutation.LastMutation]++;
                                state.Stats.MutationImprovements[mutation.LastMutation] += improvement;
                            }

                            state.CurrentMatch = mutation;
                            if (mutation.Divergence < state.BestMatch.Divergence) {
                                Debug.WriteLine("New best match found!");
                                state.SetBestMatch(mutation);
                            }
                        }
                    }
                }
            }

            Debug.WriteLine("...done running.");
        }
    }
}
