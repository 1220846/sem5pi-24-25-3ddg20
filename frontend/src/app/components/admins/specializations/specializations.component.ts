import { Component, ViewChild } from '@angular/core';
import { ListSpecializationsComponent } from './list-specializations/list-specializations.component';

@Component({
  selector: 'app-specializations',
  standalone: true,
  imports: [ListSpecializationsComponent],
  templateUrl: './specializations.component.html',
  styleUrl: './specializations.component.scss'
})
export class SpecializationsComponent {
  @ViewChild(ListSpecializationsComponent, { static: false }) listSpecializationsController!: ListSpecializationsComponent;
}
