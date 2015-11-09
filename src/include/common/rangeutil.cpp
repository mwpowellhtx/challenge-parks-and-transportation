#include "rangeutil.h"

#include <exception>

namespace challenges {

    int verify_value(int value, int expected) {
        if (value != expected) {
            //var message = string.Format("value {0} is not expected {1}", value, expected);
            //throw new ArgumentException(message, "value");
            throw std::exception();
        }
        return value;
    }

    int verify_range(int value, int min, int max) {
        if (value >= min && value <= max) return value;
        //var message = string.Format("value {0} must be in the range ({1}, {2}]", value, min, max);
        //throw new ArgumentException(message, "value");
        throw std::exception();
    }
}
