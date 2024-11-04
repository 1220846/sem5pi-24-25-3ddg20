import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TagModule } from 'primeng/tag';
import { OperationTypeService } from '../../../../services/operation-type.service';
import { OperationType } from '../../../../domain/operationType';
import { AccordionModule } from 'primeng/accordion';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { ScrollerModule } from 'primeng/scroller';

@Component({
  selector: 'app-list-operation-types',
  standalone: true,
  imports: [AccordionModule,AvatarModule,BadgeModule,TagModule,CommonModule,ScrollerModule],
  templateUrl: './list-operation-types.component.html',
  styleUrl: './list-operation-types.component.scss'
})
export class ListOperationTypesComponent implements OnInit{

  items: { label?: string; icon?: string; separator?: boolean }[] = [];
  expandedPanels: string[] = [];
  operationTypes: OperationType[] = [];

  constructor(private operationTypeService:OperationTypeService){}


  ngOnInit() {
    this.loadOperationTypes();
  }

  public loadOperationTypes(){
    
    this.operationTypeService.getAllAndFilter().subscribe((data) => {
        this.operationTypes = data;
      });
  }
}