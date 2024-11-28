import { AggregateRoot } from "../../core/domain/AggregateRoot";
import { UniqueEntityID } from "../../core/domain/UniqueEntityID";
import { Result } from "../../core/logic/Result";
import IRoomTypeDTO from "../../dto/IRoomTypeDTO";
import { RoomTypeId } from "./roomTypeId";
import { RoomTypeName } from "./roomTypeName";

interface RoomTypeProps{
    name: RoomTypeName;
}

export class RoomType extends AggregateRoot<RoomTypeProps> {
    get id (): UniqueEntityID{
        return this._id;
    }

    get roomTypeId(): RoomTypeId{
        return new RoomTypeId(this.roomTypeId.toValue());
    }

    get name(): RoomTypeName {
        return this.props.name;
    }

    private constructor (props: RoomTypeProps, id?: UniqueEntityID) {
        super(props, id);
    }

    public static create(roomTypeDTO: IRoomTypeDTO, id?: UniqueEntityID): Result<RoomType> {
        const name = RoomTypeName.create(roomTypeDTO.name);

        if (name.isFailure) {
            return Result.fail<RoomType>(name.error);
        }

        const roomType = new RoomType({ name: name.getValue() }, id);

        return Result.ok<RoomType>(roomType);
    }
}