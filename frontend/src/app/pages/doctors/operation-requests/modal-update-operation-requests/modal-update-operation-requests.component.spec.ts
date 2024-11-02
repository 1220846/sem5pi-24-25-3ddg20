import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalUpdateOperationRequestsComponent } from './modal-update-operation-requests.component';

describe('ModalUpdateOperationRequestsComponent', () => {
  let component: ModalUpdateOperationRequestsComponent;
  let fixture: ComponentFixture<ModalUpdateOperationRequestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalUpdateOperationRequestsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalUpdateOperationRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
