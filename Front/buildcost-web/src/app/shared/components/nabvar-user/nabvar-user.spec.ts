import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavbarUserComponent } from './nabvar-user';

describe('NavbarUser', () => {
  let component: NavbarUserComponent;
  let fixture: ComponentFixture<NavbarUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavbarUserComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavbarUserComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
