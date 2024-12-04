import { Request, Response, NextFunction } from 'express';

export default interface IAllergyController {
    createMedicalCondition(req: Request, res: Response, next: NextFunction);
    getMedicalCondition(req: Request, res: Response, next: NextFunction);
    //getAllergies(req: Request, res: Response, next: NextFunction);
}