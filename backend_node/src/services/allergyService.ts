import { Service, Inject } from 'typedi';
import config from "../../config";
import { Result } from "../core/logic/Result";
import IAllergyRepo from './IRepos/IAllergyRepo';
import IAllergyService from './IServices/IAllergyService';
import IAllergyDTO from '../dto/IAllergyDTO';
import { Allergy } from '../domain/allergies/allergy';
import { AllergyMap } from '../mappers/AllergyMap';
import { AllergyDesignation } from '../domain/allergies/allergyDesignation';
import { AllergyDescription } from '../domain/allergies/allergyDescription';

@Service()
export default class AllergyService implements IAllergyService {
  
  constructor(
    @Inject(config.repos.allergy.name) private allergyRepo: IAllergyRepo
  ) { }

  public async getAllergy(allergyId: string): Promise<Result<IAllergyDTO>> {
    try {
      const allergy = await this.allergyRepo.findByDomainId(allergyId);

      if (allergy === null) {
        return Result.fail<IAllergyDTO>("Allergy not found");
      }
      else {
        const allergyDTOResult = AllergyMap.toDTO(allergy) as IAllergyDTO;
        return Result.ok<IAllergyDTO>(allergyDTOResult)
      }
    } catch (e) {
      throw e;
    }
  }


  public async createAllergy(allergyDTO: IAllergyDTO): Promise<Result<IAllergyDTO>> {
    try {

      const allergyOrError = await Allergy.create(allergyDTO);

      if (allergyOrError.isFailure) {
        return Result.fail<IAllergyDTO>(allergyOrError.errorValue());
      }

      const allergyResult = allergyOrError.getValue();

      await this.allergyRepo.save(allergyResult);

      const allergyDTOResult = AllergyMap.toDTO(allergyResult) as IAllergyDTO;
      return Result.ok<IAllergyDTO>(allergyDTOResult)
    } catch (e) {
      throw e;
    }
  }

  public async getAllergies(): Promise<Result<IAllergyDTO[]>> {
    try {
      const allergies = await this.allergyRepo.findAll();

      if (!allergies || allergies.length === 0) {
        return Result.fail<IAllergyDTO[]>("No allergies found");
      }

      const allergyDTOs = allergies.map(allergy => AllergyMap.toDTO(allergy) as IAllergyDTO);

      return Result.ok<IAllergyDTO[]>(allergyDTOs);
    } catch (e) {
      throw e;
    }
  }

  public async updateAllergy(allergyDTO: IAllergyDTO): Promise<Result<IAllergyDTO>> {
      try {
        const allergy = await this.allergyRepo.findByDomainId(allergyDTO.id);
  
        if (allergy === null) {
          return Result.fail<IAllergyDTO>("Allergy not found");
        }
        else {
          if(allergyDTO.designation != null){
            allergy.designation =  AllergyDesignation.create(allergyDTO.designation).getValue();
          }

          if(allergyDTO.description != null){
            allergy.description =  AllergyDescription.create(allergyDTO.description).getValue();
          }
          
          await this.allergyRepo.save(allergy);
  
          const allergeyDTOResult = AllergyMap.toDTO( allergy ) as IAllergyDTO;
          return Result.ok<IAllergyDTO>( allergeyDTOResult )
          }
      } catch (e) {
        throw e;
      }
    }

}
