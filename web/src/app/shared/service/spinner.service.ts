import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { BehaviorSubject, Subject } from 'rxjs';
import { SpinnerComponent } from '../components/spinner/spinner.component';
import { map, scan } from 'rxjs/operators';
import { MatSpinner } from '@angular/material/progress-spinner';

@Injectable({
  providedIn: 'root'
})
export class SpinnerService {

  private spinnerTopRef = this.cdkSpinnerCreate();

    spin$ :Subject<boolean> = new Subject()

    constructor(
        private overlay: Overlay,
    ) {

      this.spin$
        .asObservable()
        .pipe(
          map(val => val ? 1 : -1 ),
          scan((acc, one) => (acc + one) >= 0 ? acc + one : 0, 0)
        )
        .subscribe(
          (res) => {
            if(res === 1){ this.start() }
            else if( res == 0 ){ 
              this.spinnerTopRef.hasAttached() ? this.stop(): null;
            }
          }
        )
    }

    private cdkSpinnerCreate() {
        return this.overlay.create({
            hasBackdrop: true,
            backdropClass: 'dark-backdrop',
            positionStrategy: this.overlay.position()
                .global()
                .centerHorizontally()
                .centerVertically()
        })
    }

    public start(){
      console.log("attach")
      this.spinnerTopRef.detach();
      this.spinnerTopRef.attach(new ComponentPortal(MatSpinner))
    }

    public stop(){
      console.log("dispose")
      this.spinnerTopRef.detach() ;
    }


  private overlayRef: OverlayRef = null;
  private dialogRef: any;
  // constructor(private router: Router, private dialog: MatDialog, private overlay: Overlay) {

  // }

  // start(message?) {
  //   if (!this.overlayRef) {
  //     this.overlayRef = this.overlay.create();
  //   }
  //   this.dialogRef = this.dialog.open(SpinnerComponent, {
  //     disableClose: false,
  //     data: message == '' || message == undefined ? "Loading..." : message
  //   });
  //   this.dialogRef;
  //   // return dialogRef;  
  //   // data: message == ''|| message == undefined ? "Loading..." : message  
  //   // const spinnerOverlayPortal = new ComponentPortal(SpinnerComponent);
  //    //const component = this.overlayRef.attach(spinnerOverlayPortal); // Attach ComponentPortal to PortalHost
  // };

  // stop(ref:MatDialogRef<SpinnerComponent>){  
  //     ref.close();  
  // } 
//   stop() {

// console.log("DialogRef",this.dialogRef);
//     if (!!this.dialogRef) {
//       this.dialogRef.close();
//     }
//     console.log("overlayRef",this.overlayRef);
//     if (!!this.overlayRef) {
//       this.overlayRef.detach();
//     }
//   }
}
