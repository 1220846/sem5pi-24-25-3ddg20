import { Mapper } from "../core/infra/Mapper";
import { Document, Model } from "mongoose";
import { UniqueEntityID } from "../core/domain/UniqueEntityID";
import { MedicalCondition } from "../domain/medicalCondition/medicalCondition";
import IMedicalConditionDTO from "../dto/IMedicalConditionDTO";
import { IMedicalConditionPersistence } from "../dataschema/IMedicalConditionPersistence";

export class MedicalConditionMap extends Mapper<MedicalCondition>{
    public static toDTO(medicalCondition : MedicalCondition): IMedicalConditionDTO{
        return {
            id: medicalCondition.id.toString(),
            code: medicalCondition.code.value,
            designation: medicalCondition.designation.value,
            description: medicalCondition.description.value
        } as IMedicalConditionDTO;
    }
    public static toDomain (medicalCondition: any | Model<IMedicalConditionPersistence & Document> ): MedicalCondition {
        const MedicalConditionOrError = MedicalCondition.create(
            medicalCondition,
            new UniqueEntityID(medicalCondition.domainId)
        );
        MedicalConditionOrError.isFailure ? console.log(MedicalConditionOrError.error) : '';
        return MedicalConditionOrError.isSuccess ? MedicalConditionOrError.getValue() : null;
    }
    public static toPersistence (medicalCondition: MedicalCondition): any {
        return {
          domainId: medicalCondition.id.toString(),
          code: medicalCondition.code.value,
          designation: medicalCondition.designation.value,
          description: medicalCondition.description.value
        }
    }
}