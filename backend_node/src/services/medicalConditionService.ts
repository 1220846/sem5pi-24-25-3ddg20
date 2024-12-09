import { Service, Inject } from 'typedi';
import config from "../../config";
import { Result } from "../core/logic/Result";
import IMedicalConditionDTO from '../dto/IMedicalConditionDTO';
import IMedicalConditionService from './IServices/IMedicalConditionService';
import IMedicalConditionRepo from './IRepos/IMedicalConditionRepo';
import { MedicalCondition } from '../domain/medicalCondition/medicalCondition';
import { MedicalConditionMap } from '../mappers/MedicalConditionMap';

@Service()
export default class MedicalConditionService implements IMedicalConditionService {
  
  constructor(
    @Inject(config.repos.medicalCondition.name) private medicalConditionRepo: IMedicalConditionRepo
  ) { }

  public async getMedicalCondition(medicalConditionId: string): Promise<Result<IMedicalConditionDTO>> {
    try {
      const medicalCondition = await this.medicalConditionRepo.findByDomainId(medicalConditionId);

      if (medicalCondition === null) {
        return Result.fail<IMedicalConditionDTO>("MedicalCondition not found");
      }
      else {
        const medicalConditionDTOResult = MedicalConditionMap.toDTO(medicalCondition) as IMedicalConditionDTO;
        return Result.ok<IMedicalConditionDTO>(medicalConditionDTOResult)
      }
    } catch (e) {
      throw e;
    }
  }


  public async createMedicalCondition(medicalConditionDTO: IMedicalConditionDTO): Promise<Result<IMedicalConditionDTO>> {
    try {

      const medicalConditionOrError = await MedicalCondition.create(medicalConditionDTO);

      if (medicalConditionOrError.isFailure) {
        return Result.fail<IMedicalConditionDTO>(medicalConditionOrError.errorValue());
      }

      const medicalConditionResult = medicalConditionOrError.getValue();

      await this.medicalConditionRepo.save(medicalConditionResult);

      const medicalConditionDTOResult = MedicalConditionMap.toDTO(medicalConditionResult) as IMedicalConditionDTO;
      return Result.ok<IMedicalConditionDTO>(medicalConditionDTOResult)
    } catch (e) {
      throw e;
    }
  }


  public async getMedicalConditions(): Promise<Result<IMedicalConditionDTO[]>> {
    try {
      const medicalCondition = await this.medicalConditionRepo.findAll();

      if (!medicalCondition || medicalCondition.length === 0) {
        return Result.fail<IMedicalConditionDTO[]>("No medicalCondition found");
      }

      const medicalConditionDTOs = medicalCondition.map(medicalCondition => MedicalConditionMap.toDTO(medicalCondition) as IMedicalConditionDTO);

      return Result.ok<IMedicalConditionDTO[]>(medicalConditionDTOs);
    } catch (e) {
      throw e;
    }
  }

}
