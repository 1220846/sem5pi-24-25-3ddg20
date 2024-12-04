import { Result } from "../../core/logic/Result";
import IAllergyDTO from "../../dto/IAllergyDTO";


export default interface IAllergyService  {
  createAllergy(AllergyDTO: IAllergyDTO): Promise<Result<IAllergyDTO>>;

  getAllergy (AllergyId: string): Promise<Result<IAllergyDTO>>;
}
