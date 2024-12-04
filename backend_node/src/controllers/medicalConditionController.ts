import { Inject, Service } from "typedi";
import { NextFunction, Request, Response } from "express";
import config from "../../config";
import { Result } from "../core/logic/Result";
import IAllergyService from "../services/IServices/IAllergyService";
import IAllergyController from "./IControllers/IAllergyController";
import IAllergyDTO from "../dto/IAllergyDTO";
import IMedicalConditionController from "./IControllers/IMedicalConditionController";
import IMedicalConditionService from "../services/IServices/IMedicalConditionService";
import IMedicalConditionDTO from "../dto/IMedicalConditionDTO";

@Service()
export default class MedicalConditionController implements IMedicalConditionController {

    constructor(
        @Inject(config.services.medicalCondition.name) private medicalConditionServiceInstance: IMedicalConditionService
    ) { }

    public async createMedicalCondition(req: Request, res: Response, next: NextFunction) {

        try {
            const medicalConditionOrError = await this.medicalConditionServiceInstance.createMedicalCondition(req.body as IMedicalConditionDTO) as Result<IMedicalConditionDTO>;

            if (medicalConditionOrError.isFailure)
                return res.status(402).send();

            const medicalConditionDTO = medicalConditionOrError.getValue();
            return res.json(medicalConditionDTO).status(201);
        } catch (e) {
            return next(e);
        }
    }
    public async getMedicalCondition(req: Request, res: Response, next: NextFunction) {

        try {
            const { id } = req.params;

            const medicalConditionOrError = await this.medicalConditionServiceInstance.getMedicalCondition(id);

            if (medicalConditionOrError.isFailure)
                return res.status(404).json({ message: medicalConditionOrError.errorValue() });

            const medicalConditionDTO = medicalConditionOrError.getValue();

            return res.status(200).json(medicalConditionDTO);
        } catch (e) {
            return next(e);
        }
    }
    /* public async getallergies(req: Request, res: Response, next: NextFunction) {
 
         try {
             const allergiesOrError = await this.medicalConditionServiceInstance.getmedicalCondition();
 
             if (allergiesOrError.isFailure)
                 return res.status(400).json({ message: allergiesOrError.errorValue() });
 
             const allergiesDTO = allergiesOrError.getValue();
             return res.status(200).json(allergiesDTO);
 
         } catch (e) {
             return next(e);
         }
     }*/
}