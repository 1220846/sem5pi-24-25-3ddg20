import { ValueObject } from "../../core/domain/ValueObject";
import { Guard } from "../../core/logic/Guard";
import { Result } from "../../core/logic/Result";

interface AllergyDesignationProps{
    value: string;
}

export class AllergyDesignation extends ValueObject<AllergyDesignationProps>{
    get value(): string{
        return this.props.value;
    }
    
    private constructor(props: AllergyDesignationProps) {
        super(props);
    }
    
    public static create(allergyDesignation: string): Result<AllergyDesignation>{
        const guardResult = Guard.againstNullOrUndefined(allergyDesignation,'AllergyDesignation')
        
        if (!guardResult.succeeded)
            return Result.fail<AllergyDesignation>(guardResult.message);
        if (!allergyDesignation.trim().length)  
            return Result.fail<AllergyDesignation>('Allergy designation cannot be empty');
        return Result.ok<AllergyDesignation>(new AllergyDesignation({ value: allergyDesignation }))
    }
}