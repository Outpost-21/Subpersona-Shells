using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace SubpersonaAI.FloatMenus
{
    public class FloatMenuOptionProvider_CarryToReconstructor : FloatMenuOptionProvider
    {
        public override bool Drafted => true;

        public override bool Undrafted => true;

        public override bool Multiselect => false;

        public override bool RequiresManipulation => true;

        public override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
        {
            if (clickedPawn.def != SubpersonaDefOf.SubAI_SubpersonaShell) { return null; }
            if (!clickedPawn.DeadOrDowned) { return null; }
            if (!context.FirstSelectedPawn.CanReserveAndReach(clickedPawn, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true)) { return null; }
            if (Building_SubpersonaReconstructor.FindReconstructorFor(clickedPawn, context.FirstSelectedPawn, true) == null) { return null; }
            TaggedString taggedString = "SubAI_CarryToSubpersonaReconstructor".Translate(clickedPawn.LabelCap, clickedPawn);
            Action action = delegate ()
            {
                Building_SubpersonaReconstructor building = Building_SubpersonaReconstructor.FindReconstructorFor(clickedPawn, context.FirstSelectedPawn, false);
                if (building == null)
                {
                    building = Building_SubpersonaReconstructor.FindReconstructorFor(clickedPawn, context.FirstSelectedPawn, true);
                }
                if (building == null)
                {
                    Messages.Message("SubAI_CannotCarryToSubpersonaReconstructor".Translate() + ": " + "SubAI_NoSubpersonaReconstructor".Translate(), clickedPawn, MessageTypeDefOf.RejectInput, historical: false);
                    return;
                }
                Job job = JobMaker.MakeJob(SubpersonaDefOf.SubAI_CarryToReconstructor, clickedPawn, building);
                job.count = 1;
                context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
            };
            return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, action, MenuOptionPriority.Default, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
        }
    }
}
