import { Result } from "../../core/logic/Result";
import IMedicalConditionDTO from "../../dto/IMedicalConditionDTO";


export default interface IMedicalConditionService  {
  createMedicalCondition(MedicalConditionDTO: IMedicalConditionDTO): Promise<Result<IMedicalConditionDTO>>;

  getMedicalCondition (MedicalConditionId: string): Promise<Result<IMedicalConditionDTO>>;
}
