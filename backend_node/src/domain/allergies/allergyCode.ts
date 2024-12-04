import { ValueObject } from "../../core/domain/ValueObject";
import { Guard } from "../../core/logic/Guard";
import { Result } from "../../core/logic/Result";

interface AllergyCodeProps{
    value: string;
}


export class AllergyCode extends ValueObject<AllergyCodeProps>{
    
    get value(): string{
        return this.props.value;
    }

    private constructor(props: AllergyCodeProps) {
        super(props);
    }
    
    public static create(allergyCode: string): Result<AllergyCode>{
        const guardResult = Guard.againstNullOrUndefined(allergyCode,'AllergyCode')
        
        if (!guardResult.succeeded)
            return Result.fail<AllergyCode>(guardResult.message);
        if (!allergyCode.trim().length)  
            return Result.fail<AllergyCode>('Allergy Code cannot be empty');
        return Result.ok<AllergyCode>(new AllergyCode({ value: allergyCode }))
    }
}