#include "extensions.h"

#include <common\rangeutil.h>

namespace challenges {

    namespace park {

        int verify_desired_attractions(int count) {
            // Bear in mind [0, 20]
            return verify_range(count, 0, 20);
        }
    }
}