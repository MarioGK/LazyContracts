using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Console = DevConsole.Console;

namespace LazyContracts
{
    public class Behaviour : ModBehaviour
    {
        private bool CannotRun => GameSettings.Instance == null || 
                                  GameSettings.Instance.MyCompany == null || 
                                  GameSettings.Instance.MyCompany.WorkItems == null;

        private bool disabled = false;
        
        public override void OnActivate()
        {
            Console.LogInfo("LazyContracts Activated!");
        }
        
        public override void OnDeactivate()
        {
            Console.LogInfo("LazyContracts Deactivated!");
        }

        private void Update()
        {
            if (CannotRun)
            {
                return;
            }

            if (disabled)
            {
                return;
            }

            /*if (!MetaData.Enabled)
            {
                return;
            }*/
            
            var workItems = GameSettings.Instance.MyCompany.WorkItems;

            AutoPromote(workItems);
            AutoBeta(workItems);
        }

        /// <summary>
        /// Auto promotes the on going contracts
        /// </summary>
        /// <param name="workItems"></param>
        private void AutoPromote(IEnumerable<WorkItem> workItems)
        {
            try
            {
                var designDocuments = workItems.OfType<DesignDocument>()
                    .Where(designDocument => designDocument.Iteration >= 1 && !designDocument.Done && designDocument.contract != null).ToList();

                foreach (var designDocument in designDocuments)
                {
                    designDocument.PromoteAction();
                }
            }
            catch (Exception e)
            {
                Console.LogError($"Error while running AutoPromote Exception: {e}");
                disabled = true;
            }
        }

        /// <summary>
        /// It handles the beta and developing cycle of the contract
        /// </summary>
        /// <param name="workItems"></param>
        private void AutoBeta(IEnumerable<WorkItem> workItems)
        {
            try
            {
                var alphaPhase = workItems.OfType<SoftwareAlpha>().Where(alpha => alpha.contract != null).ToList();

                foreach (var alpha in alphaPhase)
                {
                    if (alpha.InBeta)
                    {
                        //If it is Art related contract there is no bugs
                        if (alpha.Bugs < 0.1f)
                        {
                            alpha.PromoteAction();
                        }
                        else
                        {
                            if (alpha.FixedBugs >= Math.Min(5, alpha.Bugs / 2f))
                            {
                                alpha.PromoteAction();
                            }
                        }
                    }
                    else
                    {
                        if (alpha.GetProgress() >= Math.Min(100f,alpha.contract.MinProg * 1.2f))
                        {
                            alpha.PromoteAction();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.LogError($"Error while running AutoBeta Exception: {e}");
                disabled = true;
            }
        }
    }
}