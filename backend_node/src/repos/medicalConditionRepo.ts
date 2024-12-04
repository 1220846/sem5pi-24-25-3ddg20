import { Service, Inject } from 'typedi';
import { Document, FilterQuery, Model } from 'mongoose';
import { MedicalCondition } from '../domain/medicalCondition/medicalCondition';
import { MedicalConditionId } from '../domain/medicalCondition/medicalConditionId';
import { IMedicalConditionPersistence } from '../dataschema/IMedicalConditionPersistence';
import { MedicalConditionMap } from '../mappers/MedicalConditionMap';
import IMedicalConditionRepo from '../services/IRepos/IMedicalConditionRepo';

@Service()
export default class MedicalConsitionRepo implements IMedicalConditionRepo {
    
    private models: any;
    
    constructor(
        @Inject('medicalConditionSchema') private medicalConditionSchema: Model<IMedicalConditionPersistence & Document>,
    ) { }
    
    private createBaseQuery(): any {
        return {
            where: {},
        }
    }

    public async exists(medicalCondition: MedicalCondition): Promise<boolean> {
        const idX = medicalCondition.id instanceof MedicalConditionId ? (<MedicalConditionId>medicalCondition.id).toValue() : medicalCondition.id;
        const query = { domainId: idX };
        const medicalConditionDocument = await this.medicalConditionSchema.findOne(query as FilterQuery<IMedicalConditionPersistence & Document>);
        return !!medicalConditionDocument === true;
    }
    
    public async save(medicalCondition: MedicalCondition): Promise<MedicalCondition> {
        const query = { domainId: medicalCondition.id.toString() };
        const medicalConditionDocument = await this.medicalConditionSchema.findOne(query);
        try {
            if (medicalConditionDocument === null) {
                const rawMedicalCondition: any = MedicalConditionMap.toPersistence(medicalCondition);
                const medicalConditionCreated = await this.medicalConditionSchema.create(rawMedicalCondition);
                return MedicalConditionMap.toDomain(medicalConditionCreated);
            } else {
                medicalConditionDocument.code = medicalCondition.code.value;
                await medicalConditionDocument.save();
                return medicalCondition;
            }
        } catch (err) {
            throw err;
        }
    }
    
    public async findByDomainId(medicalConditionId: MedicalConditionId | string): Promise<MedicalCondition> {
        const query = { domainId: medicalConditionId };
        const medicalConditionRecord = await this.medicalConditionSchema.findOne(query as FilterQuery<IMedicalConditionPersistence & Document>);
        if (medicalConditionRecord != null) {
            return MedicalConditionMap.toDomain(medicalConditionRecord);
        }
        else
            return null;
    }
    
    public async findAll(): Promise<MedicalCondition[]> {
        
        const medicalConditionDocuments = await this.medicalConditionSchema.find();
        
        const medicalConditions: MedicalCondition[] = [];
        for (const medicalConditionDocument of medicalConditionDocuments) {
            const medicalCondition = await MedicalConditionMap.toDomain(medicalConditionDocument);
            if (medicalCondition) {
                medicalConditions.push(medicalCondition);
            }
        }
        
        return medicalConditions;
    }
    
}