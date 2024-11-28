import { ValueObject } from "../../core/domain/ValueObject";
import { Guard } from "../../core/logic/Guard";
import { Result } from "../../core/logic/Result";

interface RoomTypeNameProps{
    value: string;
}

export class RoomTypeName extends ValueObject<RoomTypeNameProps>{
    get value(): string{
        return this.props.value;
    }

    private constructor(props: RoomTypeNameProps) {
        super(props);
    }

    public static create(roomTypeName: string): Result<RoomTypeName>{

        const guardResult = Guard.againstNullOrUndefined(roomTypeName,'roomTypeName')
        
        if (!guardResult.succeeded)
            return Result.fail<RoomTypeName>(guardResult.message);

        if (!roomTypeName.trim().length)  
            return Result.fail<RoomTypeName>('Room type name cannot be empty');

        return Result.ok<RoomTypeName>(new RoomTypeName({ value: roomTypeName }))
    }
}

