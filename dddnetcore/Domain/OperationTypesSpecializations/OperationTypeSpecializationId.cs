using System;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Specializations;

namespace DDDSample1.Domain.OperationTypesSpecializations {
  public class OperationTypeSpecializationId : EntityId {

    public OperationTypeId OperationTypeId { get; private set; }

    public SpecializationId SpecializationId { get; private set; }

    public OperationTypeSpecializationId(OperationTypeId operationTypeId, SpecializationId specializationId) : base($"{operationTypeId.AsGuid()}-{specializationId.AsGuid()}") {
        OperationTypeId = operationTypeId;
        SpecializationId = specializationId;
    }
    public override string AsString() {
        return $"{OperationTypeId.AsGuid()}-{SpecializationId.AsGuid()}";
    }

    protected override object createFromString(String text) {
        return text;
    }

  }
}