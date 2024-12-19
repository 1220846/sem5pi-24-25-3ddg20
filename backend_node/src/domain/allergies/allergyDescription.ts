import { ValueObject } from "../../core/domain/ValueObject";
import { Result } from "../../core/logic/Result";


interface AllergyDescriptionProps{
    value: string;
}


export class AllergyDescription extends ValueObject<AllergyDescriptionProps>{
    
    get value(): string{
        return this.props.value;
    }

    private constructor(props: AllergyDescriptionProps) {
        super(props);
    }
    
    public static create(allergyDescription: string): Result<AllergyDescription>{

        return Result.ok<AllergyDescription>(new AllergyDescription({ value: allergyDescription }))
    }
}