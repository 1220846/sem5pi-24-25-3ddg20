import { ValueObject } from "../../core/domain/ValueObject";
import { Guard } from "../../core/logic/Guard";
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
        const guardResult = Guard.againstNullOrUndefined(allergyDescription,'AllergyDescription')
        
        if (!guardResult.succeeded)
            return Result.fail<AllergyDescription>(guardResult.message);
        if (!allergyDescription.trim().length)  
            return Result.fail<AllergyDescription>('Allergy description cannot be empty');
        return Result.ok<AllergyDescription>(new AllergyDescription({ value: allergyDescription }))
    }
}