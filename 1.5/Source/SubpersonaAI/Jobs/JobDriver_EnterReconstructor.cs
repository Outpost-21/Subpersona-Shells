﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;

namespace SubpersonaAI
{
    public class JobDriver_EnterReconstructor : JobDriver
	{
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
		}

		public override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil toil = Toils_General.Wait(500);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			toil.WithProgressBarToilDelay(TargetIndex.A);
			yield return toil;
			Toil enter = new Toil();
			enter.initAction = delegate
			{
				Pawn actor = enter.actor;
				Building_SubpersonaReconstructor pod = (Building_SubpersonaReconstructor)actor.CurJob.targetA.Thing;
				Action action = delegate
				{
					actor.DeSpawn();
					pod.TryAcceptThing(actor);
				};
				if (!pod.def.building.isPlayerEjectable)
				{
					if (base.Map.mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount <= 1)
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CasketWarning".Translate(actor.Named("PAWN")).AdjustedFor(actor), action));
					}
					else
					{
						action();
					}
				}
				else
				{
					action();
				}
			};
			enter.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return enter;
		}
	}
}
