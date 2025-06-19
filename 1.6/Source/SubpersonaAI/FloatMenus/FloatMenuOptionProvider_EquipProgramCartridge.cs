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
    public class FloatMenuOptionProvider_EquipProgramCartridge : FloatMenuOptionProvider
    {
        public override bool Drafted => true;

        public override bool Undrafted => true;

        public override bool Multiselect => false;

        public override bool RequiresManipulation => true;

        public override bool AppliesInt(FloatMenuContext context)
        {
            return context.FirstSelectedPawn.HasComp<Comp_ProgramCartridgeSlot>();
        }

        public override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
        {
            if (!clickedThing.def.HasModExtension<DefModExt_ProgramCartridge>()) { return null; }
            if (context.FirstSelectedPawn.def != SubpersonaDefOf.SubAI_SubpersonaShell) { return null; }
            if (context.FirstSelectedPawn.DeadOrDowned) { return null; }
            if (!context.FirstSelectedPawn.CanReserveAndReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, true)) { return null; }
            TaggedString taggedString = "Equip".Translate(clickedThing.LabelShort);
            Action action = delegate ()
            {
                Job job = JobMaker.MakeJob(SubpersonaDefOf.SubAI_GatherProgramItem, clickedThing);
                job.count = 1;
                context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
            };
            return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, action, MenuOptionPriority.Default, null, clickedThing, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
        }
    }
}
