#include "vectorutil.h"

namespace challenges {

    std::vector<int> trim_right(std::vector<int> & values, int value) {
        std::vector<int> result(values);
        while (result[result.size() - 1] == value)
            result.erase(result.begin() + result.size() - 1);
        return result;
    }
}
