import { Repo } from "../../core/infra/Repo";
import { Allergy } from "../../domain/allergies/allergy";
import { AllergyId } from "../../domain/allergies/allergyId";

export default interface IAllergyRepo extends Repo<Allergy> {
  save(allergy: Allergy): Promise<Allergy>;
  findByDomainId (allergyId: AllergyId | string): Promise<Allergy>;
  findAll();
  //findByIds (AllergysIds: AllergyId[]): Promise<Allergy[]>;
  //saveCollection (Allergys: Allergy[]): Promise<Allergy[]>;
  //removeByAllergyIds (Allergys: AllergyId[]): Promise<any>
}