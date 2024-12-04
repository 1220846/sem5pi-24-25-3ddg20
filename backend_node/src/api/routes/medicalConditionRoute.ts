import { Router } from 'express';
import Container from 'typedi';
import config from '../../../config.js';
import { celebrate, Joi } from 'celebrate';
import cors from 'cors';
import IMedicalConditionController from '../../controllers/IControllers/IMedicalConditionController.js';

const route = Router();

export default (app: Router) => {
    app.use('/medicalconditions', route);

    const ctrl = Container.get(config.controllers.medicalCondition.name) as IMedicalConditionController;

    const corsOptions = {
        origin: 'http://localhost:4200/',
        credentials: true,
        methods: ['GET', 'POST'],
        allowedHeaders: ['Content-Type', 'Authorization'],
    };

    route.post(
        '',
        cors(corsOptions),
        celebrate({
            body: Joi.object({

            }),
        }),
        (req, res, next) => ctrl.createMedicalCondition(req, res, next),
    )

    route.get('/:id', cors(corsOptions), (req, res, next) => ctrl.getMedicalCondition(req, res, next));

    //route.get('', cors(corsOptions), (req, res, next) => ctrl.getAllergies(req, res, next));
};