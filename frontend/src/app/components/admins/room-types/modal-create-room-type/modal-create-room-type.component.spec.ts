import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalCreateRoomTypeComponent } from './modal-create-room-type.component';

describe('ModalCreateRoomTypeComponent', () => {
  let component: ModalCreateRoomTypeComponent;
  let fixture: ComponentFixture<ModalCreateRoomTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalCreateRoomTypeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ModalCreateRoomTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
