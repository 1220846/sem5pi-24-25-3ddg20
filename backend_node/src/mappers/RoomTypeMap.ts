import { Document, Model } from "mongoose";
import { Mapper } from "../core/infra/Mapper";
import { IRoomTypePersistence } from "../dataschema/IRoomTypePersistence";
import { RoomType } from "../domain/roomType/roomType";
import IRoomTypeDTO from "../dto/IRoomTypeDTO";
import { UniqueEntityID } from "../core/domain/UniqueEntityID";

export class RoomTypeMap extends Mapper<RoomType>{

    public static toDTO(roomType : RoomType): IRoomTypeDTO{
        return {
            id: roomType.id.toString(),
            name: roomType.name.value,
        } as IRoomTypeDTO;
    }

    public static toDomain (roomType: any | Model<IRoomTypePersistence & Document> ): RoomType {
        const roomTypeOrError = RoomType.create(
            roomType,
            new UniqueEntityID(roomType.domainId)
        );

        roomTypeOrError.isFailure ? console.log(roomTypeOrError.error) : '';

        return roomTypeOrError.isSuccess ? roomTypeOrError.getValue() : null;
    }

    public static toPersistence (roomType: RoomType): any {
        return {
          domainId: roomType.id.toString(),
          name: roomType.name.value
        }
    }
}