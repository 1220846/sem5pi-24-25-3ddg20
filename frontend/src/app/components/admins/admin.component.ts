import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { SidebarComponent } from "../sidebar/sidebar.component";
import { ModalCreateOperationTypeComponent } from "./operation-types/modal-create-operation-type/modal-create-operation-type.component";
import { ListOperationTypesComponent } from "./operation-types/list-operation-types/list-operation-types.component";
import { OperationTypesComponent } from './operation-types/operation-types.component';
import { RouterOutlet } from '@angular/router';
import { StaffsComponent } from './staffs/staffs.component';


@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [RouterOutlet,ModalCreateOperationTypeComponent, SidebarComponent, ListOperationTypesComponent, OperationTypesComponent],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})

export class AdminComponent implements OnInit ,AfterViewInit {

  @ViewChild('sidebar') sidebar: SidebarComponent | undefined;
  @ViewChild('operation-types') operationTypes : OperationTypesComponent |  undefined;
  @ViewChild('staffs') staffs : StaffsComponent | undefined;

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.updateSidebarItems();
  }
  updateSidebarItems() {
    if (this.sidebar) {
      this.sidebar.items = [
        { label: 'Operation Types', icon: '', link: '/admin/operation-types' },
        { label: 'Staffs', icon: '', link: '/admin/staffs' }
      ];
      this.sidebar.setUserTitle('Admin');
    }
  }
}
