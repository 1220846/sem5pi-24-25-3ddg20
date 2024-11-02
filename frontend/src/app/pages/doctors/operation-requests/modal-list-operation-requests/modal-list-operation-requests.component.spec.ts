import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalListOperationRequestsComponent } from './modal-list-operation-requests.component';

describe('ModalListOperationRequestsComponent', () => {
  let component: ModalListOperationRequestsComponent;
  let fixture: ComponentFixture<ModalListOperationRequestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalListOperationRequestsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalListOperationRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
