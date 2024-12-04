import { Mapper } from "../core/infra/Mapper";
import { Document, Model } from "mongoose";
import { UniqueEntityID } from "../core/domain/UniqueEntityID";
import { Allergy } from "../domain/allergies/allergy";
import IAllergyDTO from "../dto/IAllergyDTO";
import { IAllergyPersistence } from "../dataschema/IAllergyPersistence";

export class AllergyMap extends Mapper<Allergy>{
    public static toDTO(allergy : Allergy): IAllergyDTO{
        return {
            id: allergy.id.toString(),
            code: allergy.code.value,
            designation: allergy.designation.value,
            description: allergy.description.value
        } as IAllergyDTO;
    }
    public static toDomain (allergy: any | Model<IAllergyPersistence & Document> ): Allergy {
        const AllergyOrError = Allergy.create(
            allergy,
            new UniqueEntityID(allergy.domainId)
        );
        AllergyOrError.isFailure ? console.log(AllergyOrError.error) : '';
        return AllergyOrError.isSuccess ? AllergyOrError.getValue() : null;
    }
    public static toPersistence (Allergy: Allergy): any {
        return {
          domainId: Allergy.id.toString(),
          code: Allergy.code.value,
          designation: Allergy.designation.value,
          description: Allergy.description.value
        }
    }
}