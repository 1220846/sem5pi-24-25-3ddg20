import { Router } from "express";
import Container from "typedi";
import config from "../../../config";
import IRoomTypeController from "../../controllers/IControllers/IRoomTypeController";
import { celebrate, Joi } from "celebrate";

const route = Router();

export default (app: Router) => {

    app.use('/roomtypes',route);

    const ctrl = Container.get(config.controllers.roomType.name) as IRoomTypeController;

    route.post('',
        celebrate({
            body: Joi.object({
                name: Joi.string().required(),
            })
        }),
        (req,res,next) => ctrl.addRoomType(req,res,next)
    );

    route.get('/:id', (req, res, next) => ctrl.getRoomType(req, res, next));
    route.get('', (req, res, next) => ctrl.getRoomTypes(req, res, next));
}