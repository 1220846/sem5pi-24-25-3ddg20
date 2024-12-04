import { Router } from 'express';
import Container from 'typedi';
import config from '../../../config.js';
import { celebrate, Joi } from 'celebrate';
import cors from 'cors';
import IAllergyController from '../../controllers/IControllers/IAllergyController.js';

const route = Router();

export default (app: Router) => {
    app.use('/allergies', route);

    const ctrl = Container.get(config.controllers.allergy.name) as IAllergyController;

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
                code: Joi.string().required(),
                designation: Joi.string().required(),
                description: Joi.string().required()
            }),
        }),
        (req, res, next) => ctrl.createAllergy(req, res, next),
    )

    route.get('/:id', cors(corsOptions), (req, res, next) => ctrl.getAllergy(req, res, next));

    //route.get('', cors(corsOptions), (req, res, next) => ctrl.getAllergies(req, res, next));
};