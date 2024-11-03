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

  constructor(private operationTypeService:OperationTypeService){}

  operationTypes: OperationType[] = [];

  expandedPanels: string[] = [];

  ngOnInit() {
    this.items = [
      { label: 'Refresh', icon: 'pi pi-refresh' },
      { label: 'Search', icon: 'pi pi-search' },
      { separator: true },
      { label: 'Delete', icon: 'pi pi-times' }
    ];
    this.loadOperationTypes();
  }

  loadOperationTypes(){
    this.operationTypeService.getAllAndFilter().subscribe((data) => {
        this.operationTypes = data;
      });
  }
}