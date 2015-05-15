﻿using System;

using InfinniPlatform.Api.Validation;

namespace InfinniPlatform.Api.Tests.Validation
{
	sealed class PredicateValidationOperator : BaseValidationOperator
	{
		private readonly Func<object, bool> _predicate;

		public PredicateValidationOperator(Func<object, bool> predicate)
		{
			_predicate = predicate;
		}

		protected override bool ValidateObject(object validationObject)
		{
			return _predicate(validationObject);
		}
	}
}