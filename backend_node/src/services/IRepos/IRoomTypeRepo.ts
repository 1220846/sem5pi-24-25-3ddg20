import { Repo } from "../../core/infra/Repo";
import { RoomType } from "../../domain/roomType/roomType";
import { RoomTypeId } from "../../domain/roomType/roomTypeId";

export default interface IRoomTypeRepo extends Repo<RoomType> {

    save(roomType: RoomType): Promise<RoomType>;
    findByDomainId (roomTypeId: RoomTypeId | string): Promise<RoomType>;
    findAll(): Promise<RoomType[]>;
}