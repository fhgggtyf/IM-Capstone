using System.Collections.Generic;

namespace Domicile.StateMachine
{
	public class StateTransition : IStateComponent
	{
		private State _targetState;
		private StateCondition[] _conditions;
		private int[] _resultGroups;
		private bool[] _results;

		internal StateTransition() { }
		public StateTransition(State targetState, StateCondition[] conditions, int[] resultGroups = null)
		{
			Init(targetState, conditions, resultGroups);
		}

		internal void Init(State targetState, StateCondition[] conditions, int[] resultGroups = null)
		{
			_targetState = targetState;
			_conditions = conditions;
			_resultGroups = resultGroups != null && resultGroups.Length > 0 ? resultGroups : new int[1];
			_results = new bool[_resultGroups.Length];
		}

		/// <summary>
		/// Checks wether the conditions to transition to the target state are met.
		/// </summary>
		/// <param name="state">Returns the state to transition to. Null if the conditions aren't met.</param>
		/// <returns>True if the conditions are met.</returns>
		public bool TryGetTransiton(out State state)
		{
			state = ShouldTransition() ? _targetState : null;
			return state != null;
		}

		public void OnStateEnter()
		{
			for (int i = 0; i < _conditions.Length; i++)
				_conditions[i]._condition.OnStateEnter();
		}

		public void OnStateExit()
		{
			for (int i = 0; i < _conditions.Length; i++)
				_conditions[i]._condition.OnStateExit();
		}

		private bool ShouldTransition()
		{
#if UNITY_EDITOR
            _targetState._stateMachine._debugger.TransitionEvaluationBegin(_targetState._originSO.name);
#endif
            //bool isCritical = false;
            //bool criticalFoundFlag = false;
            //List<Condition> criticalFailedConditionsList = new List<Condition>();
            int count = _resultGroups.Length;
            for (int i = 0, idx = 0; i < count && idx < _conditions.Length; i++)
            {
                for (int j = 0; j < _resultGroups[i]; j++, idx++)
                {

                    //bool conditionMet = _conditions[idx].IsMet();
                    _results[i] = j == 0 ? _conditions[idx].IsMet() : _results[i] && _conditions[idx].IsMet();
                    //if (isCritical && conditionMet)
                    //{
                    //    criticalFoundFlag = true;
                    //    for (int n = 0; n < j; n++)
                    //    {
                    //        if (!_conditions[n].IsMet(out isCritical))
                    //        {
                    //            criticalFailedConditionsList.Add(_conditions[idx]._condition);
                    //        }
                    //    }
                    //}

                    //if (j == 0)
                    //{
                    //    _results[i] = conditionMet/*_conditions[idx].IsMet(/*out isCritical)*/;
                    //}
                    //else
                    //{
                    //    _results[i] = _results[i] && conditionMet /*_conditions[idx].IsMet(/*out isCritical)*/;
                    //}

                    //if (criticalFoundFlag && !conditionMet)
                    //{
                    //	criticalFailedConditionsList.Add(_conditions[idx]._condition);
                    //}
                }
                //criticalFoundFlag = false;

            }


            //store idx if got essential con
            //use idx to find result group number
            //check if result group is satisfied
            //if not then look for conditions not sat
            //run code for cons not sat


            bool ret = false;
			for (int i = 0; i < count && !ret; i++)
				ret = ret || _results[i];

#if UNITY_EDITOR
			_targetState._stateMachine._debugger.TransitionEvaluationEnd(ret, _targetState._actions);
#endif

			//if (!ret && criticalFailedConditionsList.Count > 0)
   //         {
			//	foreach(var condition in criticalFailedConditionsList)
   //             {
			//		condition.OnTransitionFailed();
   //             }
   //         }

			return ret;
		}

		internal void ClearConditionsCache()
		{
			for (int i = 0; i < _conditions.Length; i++)
				_conditions[i]._condition.ClearStatementCache();
		}
	}
}
