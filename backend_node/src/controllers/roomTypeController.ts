import { Inject, Service } from "typedi";
import IRoomTypeController from "./IControllers/IRoomTypeController";
import { NextFunction, Request, Response } from "express";
import config from "../../config";
import IRoomTypeService from "../services/IServices/IRoomTypeService";
import IRoomTypeDTO from "../dto/IRoomTypeDTO";
import { Result } from "../core/logic/Result";

@Service()
export default class RoomTypeController implements IRoomTypeController{
    
    constructor(
        @Inject(config.services.roomType.name) private roomTypeServiceInstance : IRoomTypeService
    ) {}
    
    public async addRoomType(req: Request, res: Response, next: NextFunction) {
        
        try{
            const roomTypeOrError = await this.roomTypeServiceInstance.addRoomType(req.body as IRoomTypeDTO) as Result<IRoomTypeDTO>;
            
            if (roomTypeOrError.isFailure)
                return res.status(402).send();
        
            const roomTypeDTO = roomTypeOrError.getValue();
            return res.json( roomTypeDTO ).status(201);

        } catch (e) {
            return next(e);
        }
    }

    public async getRoomType(req: Request, res: Response, next: NextFunction) {
        
        try {
            const { id } = req.params;
    
            const roomTypeOrError = await this.roomTypeServiceInstance.getRoomType(id);
    
            if (roomTypeOrError.isFailure)
                return res.status(404).json({ message: roomTypeOrError.errorValue() });
    
            const roomTypeDTO = roomTypeOrError.getValue();
    
            return res.status(200).json(roomTypeDTO);

        } catch (e) {
            return next(e);
        }
    }

    public async getRoomTypes(req: Request, res: Response, next: NextFunction){
        
        try {
            const roomTypesOrError = await this.roomTypeServiceInstance.getRoomTypes();
    
            if (roomTypesOrError.isFailure)
                return res.status(400).json({ message: roomTypesOrError.errorValue() });
    
            const roomTypesDTO = roomTypesOrError.getValue();

            return res.status(200).json(roomTypesDTO);
        
        } catch (e) {
            return next(e);
        }
    }

}