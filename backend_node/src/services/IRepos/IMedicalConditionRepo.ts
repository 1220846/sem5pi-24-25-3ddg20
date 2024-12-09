import { Repo } from "../../core/infra/Repo";
import { MedicalCondition } from "../../domain/medicalCondition/medicalCondition";
import { MedicalConditionId } from "../../domain/medicalCondition/medicalConditionId";

export default interface IMedicalConditionRepo extends Repo<MedicalCondition> {
  save(medicalCondition: MedicalCondition): Promise<MedicalCondition>;
  findByDomainId (medicalConditionId: MedicalConditionId | string): Promise<MedicalCondition>;
  findAll();
  //findByIds (MedicalConditionsIds: MedicalConditionId[]): Promise<MedicalCondition[]>;
  //saveCollection (MedicalConditions: MedicalCondition[]): Promise<MedicalCondition[]>;
  //removeByMedicalConditionIds (MedicalConditions: MedicalConditionId[]): Promise<any>
}