import { Result } from "../../core/logic/Result";
import IRoomTypeDTO from "../../dto/IRoomTypeDTO";

export default interface IRoomTypeService  {
    addRoomType(roomTypeDTO: IRoomTypeDTO): Promise<Result<IRoomTypeDTO>>;
    getRoomType (roomTypeId: string): Promise<Result<IRoomTypeDTO>>;
    getRoomTypes(): Promise<Result<IRoomTypeDTO[]>>;
}
