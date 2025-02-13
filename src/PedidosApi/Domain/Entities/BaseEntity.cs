﻿using FluentValidation.Results;

namespace PedidosApi.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public bool IsValid() => !Validate().Any();

        public IEnumerable<string> Validate()
        {
            var validations = GetValidation();

            if (validations.IsValid)
            {
                yield break;
            }

            foreach (var error in validations.Errors)
            {
                yield return (error.ErrorMessage);
            }
        }

        protected abstract ValidationResult GetValidation();
    }
}
