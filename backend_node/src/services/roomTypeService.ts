import { Inject, Service } from "typedi";
import IRoomTypeService from "./IServices/IRoomTypeService";
import IRoomTypeRepo from "./IRepos/IRoomTypeRepo";
import config from "../../config";
import { Result } from "../core/logic/Result";
import IRoomTypeDTO from "../dto/IRoomTypeDTO";
import { RoomTypeMap } from "../mappers/RoomTypeMap";
import { RoomType } from "../domain/roomType/roomType";

@Service()
export default class RoomTypeService implements IRoomTypeService{
    constructor(
        @Inject(config.repos.roomType.name) private roomTypeRepo: IRoomTypeRepo
    ){}

    public async getRoomType(roomTypeId: string): Promise<Result<IRoomTypeDTO>> {
        
        try{
            const roomType = await this.roomTypeRepo.findByDomainId(roomTypeId);

            if(roomType === null)
                return Result.fail<IRoomTypeDTO>("Room Type not found")
            
            const roomTypeDTOResult = RoomTypeMap.toDTO(roomType) as IRoomTypeDTO;
            
            return Result.ok<IRoomTypeDTO>(roomTypeDTOResult);
        } catch(e){
            throw e;
        }
    }

    public async addRoomType(roomTypeDTO: IRoomTypeDTO): Promise<Result<IRoomTypeDTO>> {

        try{

            const roomTypeOrError = await RoomType.create(roomTypeDTO);

            if(roomTypeOrError.isFailure)
                return Result.fail<IRoomTypeDTO>(roomTypeOrError.errorValue);

            const roomTypeResult = roomTypeOrError.getValue();

            await this.roomTypeRepo.save(roomTypeResult);
            
            const roomTypeDTOResult = RoomTypeMap.toDTO(roomTypeResult) as IRoomTypeDTO;

            return Result.ok<IRoomTypeDTO>(roomTypeDTOResult);
    
        } catch (e) {
            throw e;
        }
    }

    public async getRoomTypes(): Promise<Result<IRoomTypeDTO[]>> {

        try {
            const roomTypes = await this.roomTypeRepo.findAll();

            const roomTypesDTOs = roomTypes.map(roomType => RoomTypeMap.toDTO(roomType) as IRoomTypeDTO);
            return Result.ok<IRoomTypeDTO[]>(roomTypesDTOs);

        } catch (e) {
            throw e;
        }
    }
}