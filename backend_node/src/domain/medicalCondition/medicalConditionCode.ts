import { ValueObject } from "../../core/domain/ValueObject";
import { Guard } from "../../core/logic/Guard";
import { Result } from "../../core/logic/Result";

interface MedicalConditionCodeProps{
    value: string;
}


export class MedicalConditionCode extends ValueObject<MedicalConditionCodeProps>{
    
    get value(): string{
        return this.props.value;
    }

    private constructor(props: MedicalConditionCodeProps) {
        super(props);
    }
    
    public static create(medicalConditionCode: string): Result<MedicalConditionCode>{
        const guardResult = Guard.againstNullOrUndefined(medicalConditionCode,'MedicalConditionCode')
        
        if (!guardResult.succeeded)
            return Result.fail<MedicalConditionCode>(guardResult.message);
        if (!medicalConditionCode.trim().length)  
            return Result.fail<MedicalConditionCode>('Medical Condition Code cannot be empty');
        return Result.ok<MedicalConditionCode>(new MedicalConditionCode({ value: medicalConditionCode }))
    }
}