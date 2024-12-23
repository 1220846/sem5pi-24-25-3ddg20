import { Inject, Service } from "typedi";
import { NextFunction, Request, Response } from "express";
import config from "../../config";
import { Result } from "../core/logic/Result";
import IAllergyService from "../services/IServices/IAllergyService";
import IAllergyController from "./IControllers/IAllergyController";
import IAllergyDTO from "../dto/IAllergyDTO";

@Service()
export default class AllergyController implements IAllergyController {

    constructor(
        @Inject(config.services.allergy.name) private allergyServiceInstance: IAllergyService
    ) { }

    public async createAllergy(req: Request, res: Response, next: NextFunction) {

        try {
            const allergyOrError = await this.allergyServiceInstance.createAllergy(req.body as IAllergyDTO) as Result<IAllergyDTO>;

            if (allergyOrError.isFailure)
                return res.status(400).send();

            const allergyDTO = allergyOrError.getValue();
            return res.json(allergyDTO).status(201);
        } catch (e) {
            return next(e);
        }
    }
    public async getAllergy(req: Request, res: Response, next: NextFunction) {

        try {
            const { id } = req.params;

            const allergyOrError = await this.allergyServiceInstance.getAllergy(id);

            if (allergyOrError.isFailure)
                return res.status(404).json({ message: allergyOrError.errorValue() });

            const allergyDTO = allergyOrError.getValue();

            return res.status(200).json(allergyDTO);
        } catch (e) {
            return next(e);
        }
    }
    public async getAllergies(req: Request, res: Response, next: NextFunction) {
 
        try {
            const allergiesOrError = await this.allergyServiceInstance.getAllergies();

            if (allergiesOrError.isFailure)
                return res.status(400).json({ message: allergiesOrError.errorValue() });

            const allergiesDTO = allergiesOrError.getValue();
            return res.status(200).json(allergiesDTO);

        } catch (e) {
            return next(e);
        }
    }

    public async updateAllergy(req: Request, res: Response, next: NextFunction) {
        try {
            console.log('Controller ID:', req.params.id);
            console.log('Controller Request Body:', req.body);
            console.log('Received ID:', req.params.id); // Verifica o id passado na URL

            const allergyOrError = await this.allergyServiceInstance.updateAllergy(req.body as IAllergyDTO) as Result<IAllergyDTO>;

            if (allergyOrError.isFailure) {
                if (!allergyOrError.error.toString().includes("Allergy")) {
                    return res.status(500).json({ message: "An unexpected error occurred." });
                }
                return res.status(400).json({ message: allergyOrError.error });
            }

            const roleDTO = allergyOrError.getValue();
            return res.status(200).json( roleDTO );
        }
        catch (e) {
            return next(e);
        }
    };
}