import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DatosM2 } from './datos-m2';

describe('DatosM2', () => {
  let component: DatosM2;
  let fixture: ComponentFixture<DatosM2>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DatosM2],
    }).compileComponents();

    fixture = TestBed.createComponent(DatosM2);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
