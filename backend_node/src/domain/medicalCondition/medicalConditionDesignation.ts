import { ValueObject } from "../../core/domain/ValueObject";
import { Guard } from "../../core/logic/Guard";
import { Result } from "../../core/logic/Result";

interface MedicalConditionDesignationProps{
    value: string;
}

export class MedicalConditionDesignation extends ValueObject<MedicalConditionDesignationProps>{
    get value(): string{
        return this.props.value;
    }
    
    private constructor(props: MedicalConditionDesignationProps) {
        super(props);
    }
    
    public static create(medicalConditionDesignation: string): Result<MedicalConditionDesignation>{
        const guardResult = Guard.againstNullOrUndefined(medicalConditionDesignation,'MedicalConditionDesignation')
        
        if (!guardResult.succeeded)
            return Result.fail<MedicalConditionDesignation>(guardResult.message);
        if (!medicalConditionDesignation.trim().length)  
            return Result.fail<MedicalConditionDesignation>('Medical Condition designation cannot be empty');
        return Result.ok<MedicalConditionDesignation>(new MedicalConditionDesignation({ value: medicalConditionDesignation }))
    }
}