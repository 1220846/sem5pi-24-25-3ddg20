import { ValueObject } from "../../core/domain/ValueObject";
import { Guard } from "../../core/logic/Guard";
import { Result } from "../../core/logic/Result";


interface MedicalConditionDescriptionProps{
    value: string;
}


export class MedicalConditionDescription extends ValueObject<MedicalConditionDescriptionProps>{
    
    get value(): string{
        return this.props.value;
    }

    private constructor(props: MedicalConditionDescriptionProps) {
        super(props);
    }
    
    public static create(medicalConditionDescription: string): Result<MedicalConditionDescription>{
        const guardResult = Guard.againstNullOrUndefined(medicalConditionDescription,'MedicalConditionDescription')
        
        if (!guardResult.succeeded)
            return Result.fail<MedicalConditionDescription>(guardResult.message);
        if (!medicalConditionDescription.trim().length)  
            return Result.fail<MedicalConditionDescription>('Medical Condition description cannot be empty');
        return Result.ok<MedicalConditionDescription>(new MedicalConditionDescription({ value: medicalConditionDescription }))
    }
}