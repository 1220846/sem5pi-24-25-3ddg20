import { AggregateRoot } from "../../core/domain/AggregateRoot";
import { UniqueEntityID } from "../../core/domain/UniqueEntityID";
import { Result } from "../../core/logic/Result";
import IMedicalConditionDTO from "../../dto/IMedicalConditionDTO";
import { MedicalConditionCode } from "./medicalConditionCode";
import { MedicalConditionDescription } from "./medicalConditionDescription";
import { MedicalConditionDesignation } from "./medicalConditionDesignation";


interface MedicalConditionProps{
    code: MedicalConditionCode;
    description: MedicalConditionDescription;
    designation: MedicalConditionDesignation;
}

export class MedicalCondition extends AggregateRoot<MedicalConditionProps> {
    
    get id (): UniqueEntityID{
        return this._id;
    }

    get code(): MedicalConditionCode{
        return this.props.code;
    }

    get description(): MedicalConditionDescription{
        return this.props.description;
    }
    
    get designation(): MedicalConditionDesignation {
        return this.props.designation;
    }

    private constructor (props: MedicalConditionProps, id?: UniqueEntityID) {
        super(props, id);
    }
    public static create(medicalConditionDTO: IMedicalConditionDTO, id?: UniqueEntityID): Result<MedicalCondition> {

        const code = MedicalConditionCode.create(medicalConditionDTO.code);
        if (code.isFailure) {
            return Result.fail<MedicalCondition>(code.error);
        }

        const designation = MedicalConditionDesignation.create(medicalConditionDTO.designation);
        if (designation.isFailure) {
            return Result.fail<MedicalCondition>(designation.error);
        }

        const description = MedicalConditionDescription.create(medicalConditionDTO.description);
        if (description.isFailure) {
            return Result.fail<MedicalCondition>(description.error);
        }

        const medicalCondition = new MedicalCondition({ code : code.getValue(), description: description.getValue(), designation: designation.getValue()}, id);
        return Result.ok<MedicalCondition>(medicalCondition);
    }
}