import { AggregateRoot } from "../../core/domain/AggregateRoot";
import { UniqueEntityID } from "../../core/domain/UniqueEntityID";
import { Result } from "../../core/logic/Result";
import IAllergyDTO from "../../dto/IAllergyDTO";
import { AllergyCode } from "./allergyCode";
import { AllergyDescription } from "./allergyDescription";
import { AllergyDesignation } from "./allergyDesignation";
import { AllergyId } from "./allergyId";

interface AllergyProps{
    code: AllergyCode;
    description: AllergyDescription;
    designation: AllergyDesignation;
}

export class Allergy extends AggregateRoot<AllergyProps> {
    
    get id (): UniqueEntityID{
        return this._id;
    }

    get code(): AllergyCode{
        return this.props.code;
    }

    get description(): AllergyDescription{
        return this.props.description;
    }
    
    get designation(): AllergyDesignation {
        return this.props.designation;
    }

    set designation ( value: AllergyDesignation) {
        this.props.designation = value;
    }

    set description ( value: AllergyDescription) {
        this.props.description = value;
    }

    private constructor (props: AllergyProps, id?: UniqueEntityID) {
        super(props, id);
    }

    public static create(AllergyDTO: IAllergyDTO, id?: UniqueEntityID): Result<Allergy> {

        const code = AllergyCode.create(AllergyDTO.code);
        if (code.isFailure) {
            return Result.fail<Allergy>(code.error);
        }

        const designation = AllergyDesignation.create(AllergyDTO.designation);
        if (designation.isFailure) {
            return Result.fail<Allergy>(designation.error);
        }

        const description = AllergyDescription.create(AllergyDTO.description);
        if (description.isFailure) {
            return Result.fail<Allergy>(description.error);
        }

        const allergy = new Allergy({ code : code.getValue(), description: description.getValue(), designation: designation.getValue()}, id);
        return Result.ok<Allergy>(allergy);
    }
}