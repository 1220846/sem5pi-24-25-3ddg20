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
import { Console } from "console";

@Service()
export default class MedicalConditionController implements IMedicalConditionController {

    constructor(
        @Inject(config.services.medicalCondition.name) private medicalConditionServiceInstance: IMedicalConditionService
    ) { }

    public async createMedicalCondition(req: Request, res: Response, next: NextFunction) {
        try {
            const medicalConditionOrError = await this.medicalConditionServiceInstance.createMedicalCondition(req.body as IMedicalConditionDTO) as Result<IMedicalConditionDTO>;
            
            if (medicalConditionOrError.isFailure) {
                if (!medicalConditionOrError.error.toString().includes("Medical Condition")) {
                    return res.status(500).json({ message: "An unexpected error occurred." });
                }
                return res.status(400).json({ message: medicalConditionOrError.error });
            }


            const medicalConditionDTO = medicalConditionOrError.getValue();
            return res.status(201).json(medicalConditionDTO);
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
    public async getMedicalConditions(req: Request, res: Response, next: NextFunction) {
 
        try {
            const medicalConditionsOrError = await this.medicalConditionServiceInstance.getMedicalConditions();

            if (medicalConditionsOrError.isFailure)
                return res.status(400).json({ message: medicalConditionsOrError.errorValue() });

            const medicalConditionsDTO = medicalConditionsOrError.getValue();
            return res.status(200).json(medicalConditionsDTO);

        } catch (e) {
            return next(e);
        }
    }
}